namespace MyClassLibrary.LocalServerMethods.Interfaces
{
    /// <summary>
    /// The interface providing the model for updates that are the building blocks of the local server system.
    /// </summary>
    public interface ILocalServerModelUpdate
    {
        bool IsConflicted { get; set; }
        DateTime Created { get; set; }
        string CreatedBy { get; set; }
        Guid Id { get; set; }
        bool IsActive { get; set; }
        DateTime? UpdatedOnServer { get; set; }
    }
}