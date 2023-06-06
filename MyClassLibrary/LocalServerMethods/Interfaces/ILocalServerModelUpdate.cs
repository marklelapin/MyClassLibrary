﻿namespace MyClassLibrary.LocalServerMethods.Interfaces
{
    /// <summary>
    /// The interface providing the model for updates that are the building blocks of the local server system.
    /// </summary>
    public interface ILocalServerModelUpdate
    {
        /// <summary>
        /// Indicates if other updates exist that were created locally and seperately on different local machines before syncing could take place.
        /// </summary>
        bool IsConflicted { get; set; }
        /// <summary>
        /// The date and time the update was created on the local machine.
        /// </summary>
        DateTime Created { get; set; }
        /// <summary>
        /// The user who created the update.
        /// </summary>
        string CreatedBy { get; set; }
        /// <summary>
        /// The id of the model for which this update relates.
        /// </summary>
        Guid Id { get; set; }
        /// <summary>
        /// Indicates wether or not the model should be generally viewed. (Soft Delete)
        /// </summary>
        bool IsActive { get; set; }
        /// <summary>
        /// The date and time that the update was saved to the central server storage.
        /// </summary>
        DateTime? UpdatedOnServer { get; set; }
    }
}