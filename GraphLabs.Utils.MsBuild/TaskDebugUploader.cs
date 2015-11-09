using System;
using System.Diagnostics;
using System.IO;
using System.ServiceModel;
using GraphLabs.Utils.MsBuild.DebugTaskUploader;
using Microsoft.Build.Utilities;

namespace GraphLabs.Utils.MsBuild
{
    /// <summary> Загрузчик модулей-заданий для отладки </summary>
    public class TaskDebugUploader : Task
    {
        #region Параметры задания

        /// <summary> Путь к xap-файлу задания </summary>
        public string XapPath { get; set; }

        /// <summary> Путь к бинарным данным варианта </summary>
        public string VariantDataPath { get; set; }

        /// <summary> Путь к сервису загрузки </summary>
        public string UploadServiceUri { get; set; }

        /// <summary> Путь к страницы выполнения лабы </summary>
        /// <remarks> Пример: http://glservice.svtz.ru:81/LabWorkExecution/Index?labId={0}&amp;labVarId={1} </remarks>
        public string LabUriFormat { get; set; }

        #endregion


        private byte[] GetXap()
        {
            if (string.IsNullOrEmpty(XapPath))
            {
                throw new ArgumentException($"Не указан параметр {nameof(XapPath)} - путь к модулю-заданию для загрузки.");
            }

            if (!File.Exists(XapPath))
            {
                throw new FileNotFoundException("Файл с модулем-заданием не найден.", XapPath);
            }

            try
            {
                return File.ReadAllBytes(XapPath);
            }
            catch (Exception ex)
            {
                throw new IOException("Не удалось прочитать файл модуля-задания.", ex);
            }
        }

        private byte[] GetVariant()
        {
            if (string.IsNullOrEmpty(VariantDataPath))
            {
                throw new ArgumentException($"Не указан параметр {nameof(VariantDataPath)} - путь к данным варианта для загрузки.");
            }

            if (!File.Exists(VariantDataPath))
            {
                throw new FileNotFoundException("Файл с данными варианта не найден.", VariantDataPath);
            }

            try
            {
                return File.ReadAllBytes(VariantDataPath);
            }
            catch (Exception ex)
            {
                throw new IOException("Не удалось прочитать файл варианта.", ex);
            }
        }

        public override bool Execute()
        {
            var address = new EndpointAddress(UploadServiceUri);
            var binding = new BasicHttpBinding();
            var proxy = new DebugTaskUploaderClient(binding, address);

            var x = GetXap();
            var v = GetVariant();

            var response = proxy.UploadDebugTask(x, v);

            if (!string.IsNullOrEmpty(LabUriFormat))
            {
                var uri = string.Format(LabUriFormat, response.LabWorkId, response.LabVariantId);
                Process.Start(uri);
            }

            return true;
        }
    }
}
