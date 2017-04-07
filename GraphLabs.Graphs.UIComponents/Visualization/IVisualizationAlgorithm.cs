namespace GraphLabs.Graphs.UIComponents.Visualization
{
    /// <summary>
    /// можно сделать свою визуализацию
    /// </summary>
    public interface IVisualizationAlgorithm
    {
        /// <summary>
        /// имя алгоритма визуализации
        /// </summary>
        /// <returns></returns>
        string Name();

        /// <summary>
        /// то как мы выпоняем визуализацию
        /// </summary>
        void Visualize();

        /// <summary> временно </summary>
        GraphVisualizer Visualizer { get; set; }
    }
}