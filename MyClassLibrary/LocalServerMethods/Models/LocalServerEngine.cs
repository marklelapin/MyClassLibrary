using MyClassLibrary.LocalServerMethods.Interfaces;
using System.Data.SqlClient;

namespace MyClassLibrary.LocalServerMethods.Models
{
	public class LocalServerEngine<T> : ILocalServerEngine<T> where T : ILocalServerModelUpdate, new()
	{


		private ILocalDataAccess<T> _localDataAccess;

		private IServerDataAccess<T> _serverDataAccess;

		/// <summary>
		/// Constructor for LocalServerEngine.
		/// </summary>
		public LocalServerEngine(IServerDataAccess<T> serverDataAccess, ILocalDataAccess<T> localDataAccess)
		{
			_serverDataAccess = serverDataAccess;
			_localDataAccess = localDataAccess;
		}



		//GetAll________________________________________________________________________
		public async Task<List<T>> GetAllUpdates()
		{
			List<Guid>? ids = null;
			return await GetAllUpdates(ids);
		}

		public async Task<List<T>> GetAllUpdates(Guid id)
		{
			return await GetAllUpdates(new List<Guid> { id });
		}

		public async Task<List<T>> GetAllUpdates(List<Guid>? ids = null)
		{

			List<T> output = new List<T>();

			await TrySync();

			output = await _localDataAccess.GetUpdatesFromLocal(ids, false);

			return output;
		}


		//GetConflicted______________________________________________________________-_
		public async Task<List<T>> GetConflictedUpdates()
		{
			List<Guid>? ids = null;
			return await GetConflictedUpdates(ids);
		}

		public async Task<List<T>> GetConflictedUpdates(Guid id)
		{
			return await GetConflictedUpdates(new List<Guid> { id });
		}

		public async Task<List<T>> GetConflictedUpdates(List<Guid>? ids)
		{
			List<T> output = new List<T>();

			await TrySync();

			output = await _localDataAccess.GetConflictedUpdatesFromLocal(ids);

			return output;
		}


		//GetLatest_____________________________________________________________________
		public async Task<List<T>> GetLatestUpdates()
		{
			List<Guid>? ids = null;
			return await GetLatestUpdates(ids);
		}

		public async Task<List<T>> GetLatestUpdates(Guid id)
		{
			return await GetLatestUpdates(new List<Guid> { id });
		}

		public async Task<List<T>> GetLatestUpdates(List<Guid>? ids = null)
		{

			List<T> output = new List<T>();

			await TrySync();

			output = await _localDataAccess.GetUpdatesFromLocal(ids, true);

			return output;
		}

		//SaveUpdates__________________________________________________________________
		public async Task SaveUpdates(T update, bool syncAfterwards = true)
		{
			await SaveUpdates(new List<T> { update });
		}
		public async Task SaveUpdates(List<T> updates, bool syncAfterwards = true)
		{
			await _localDataAccess.SaveUpdatesToLocal(updates);

			if (syncAfterwards)
			{
				await TrySync();
			}

		}




		//Sync________________________________________________________________________
		public async Task<(DateTime? syncedDateTime, bool success)> TrySync()
		{


			if (await TrySyncServerToLocal() && await TrySyncLocalToServer())
			{
				DateTime syncedDateTime = DateTime.Now;
				await _localDataAccess.SaveLocalLastSyncDate(syncedDateTime);
				return (syncedDateTime, true);
			}
			else
			{
				return (null, false);
			}

		}

		private async Task<bool> TrySyncServerToLocal()
		{
			try
			{
				Guid copyId = await _localDataAccess.GetLocalCopyID();

				List<T> unsyncedServerUpdates = await _serverDataAccess.GetUnsyncedFromServer(copyId);

				if (unsyncedServerUpdates.Count > 0)
				{
					List<LocalToServerPostBack> postBack = await _localDataAccess.SaveUpdatesToLocal(unsyncedServerUpdates);

					await _serverDataAccess.LocalPostBackToServer(postBack, copyId);
				}


				return true;
			}
			catch (Exception ex) { return await SyncErrorHandler(ex); };


		}

		private async Task<bool> TrySyncLocalToServer()
		{
			try
			{
				Guid copyId = await _localDataAccess.GetLocalCopyID();

				List<T> unsyncedLocalUpdates = await _localDataAccess.GetUnsyncedFromLocal();

				if (unsyncedLocalUpdates.Count > 0)
				{
					List<ServerToLocalPostBack> postBack = await _serverDataAccess.SaveUpdatesToServer(unsyncedLocalUpdates, copyId);

					await _localDataAccess.ServerPostBackToLocal(postBack);
				}


				return true;
			}
			catch (Exception ex) { return await SyncErrorHandler(ex); };

		}

		private async Task<bool> SyncErrorHandler(Exception ex)
		{


			await Task.Run(() =>
			{
				if (ex is SqlException sqlEx)
				{
					if (sqlEx.Number == 53)
					{
						//Do nothing. It is assumed that connectivity issues exist and when the applicaiton comes back on line it will resync.
					}
				}
				else
				{
					throw ex;
				}
			});

			return false;
		}



		////TODO - Make final decision on removing this functionality entirely.
		////DeleteEntirely_____________________________________________________________
		//public async Task DeleteEntirely(List<T> updates)  //TODO Unit Test this
		//        {
		//             await TryDeleteFromLocal(updates);
		//             await TryDeleteFromServer(updates);//TODO need to think through what happens if this fails. - won't get pickedup in any future updates.
		//                                                //Add in something to update that tells server to delete??      
		//}









		public async Task<bool> ClearConflictIds(Guid id)
		{
			return await ClearConflictIds(new List<Guid> { id });
		}

		public async Task<bool> ClearConflictIds(List<Guid> Ids)
		{
			try
			{
				await _localDataAccess.ClearConflictsFromLocal(Ids);
				await _serverDataAccess.ClearConflictsFromServer(Ids);

				return true;
			}
			catch (Exception ex) { return await SyncErrorHandler(ex); } //TODO improve ErrorHandling consistency??

		}



		public void ChangeLocalDataAccess(ILocalDataAccess<T> localDataAccess)
		{
			_localDataAccess = localDataAccess;
		}

		public void ChangeServerDataAccess(IServerDataAccess<T> serverDataAccess)
		{
			_serverDataAccess = serverDataAccess;
		}







		//Private Methods________________________________________________________________
		////TODO - Make final decision on removing DeleteEntirely functionality.
		///// <summary>
		///// Tries to Save Updates to Local. If it fails returns false.
		///// </summary>
		//private async Task<bool> TryDeleteFromLocal(List<T> updates)
		//{
		//    try
		//    {
		//        await _localDataAccess.DeleteFromLocal(updates);
		//        return true;
		//    }
		//    catch
		//    {
		//        return false;
		//        Try Log Delete from Local Error
		//    }
		//}
		///// <summary>
		///// Tries to Save Updates to Server. If it fails returns false.
		///// </summary>
		//private async Task<bool> TryDeleteFromServer(List<T> updates)
		//{
		//    try
		//    {
		//        await _serverDataAccess.DeleteFromServer(updates);
		//        return true;
		//    }
		//    catch
		//    {
		//        return false;
		//        Try Log Delete from Server Error
		//    }
		//}


	}

}

