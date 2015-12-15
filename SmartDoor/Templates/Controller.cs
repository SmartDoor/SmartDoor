namespace SmartDoor.Templates
{
    /// <summary>
    /// An interface to define what a controller
    /// is the common methods shared in them.
    /// </summary>
    interface Controller
    {
        void Setup();
        void Shutdown();
    }
}
