using MyClassLibrary.LocalServerMethods.Interfaces;


namespace MyClassLibrary.LocalServerMethods.Models
{

	public class LocalServerModel<T> : ILocalServerModel<T> where T : ILocalServerModelUpdate, new()
	{
		/// <summary>
		/// The Identity of the LocalServerIdentity
		/// </summary>
		public Guid Id { get; set; } = Guid.Empty;

		/// <summary>
		/// The Latest update to the LocalServerIdentity.
		/// </summary>
		/// <return>
		/// Returns null if the latest update is deactivated.
		/// </return>
		public T? Latest { get; set; }
		/// <summary>
		/// All the updates with the same conflictId as the latest update for the LocalServerIdentity
		/// </summary>
		public List<T>? Conflicts { get; set; }
		/// <summary>
		/// All updates relating to the LocalServerIdentity
		/// </summary>
		public List<T>? History { get; set; }


		public LocalServerModel()
		{

		}

		public LocalServerModel(T update)
		{
			Id = update.Id;
			Latest = update;
			Conflicts = new List<T>();
			History = new List<T>() { update };
		}
	}

}

