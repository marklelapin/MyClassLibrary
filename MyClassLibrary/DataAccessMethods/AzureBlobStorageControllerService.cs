using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyClassLibrary.Interfaces;

namespace MyClassLibrary.DataAccessMethods
{
	public class AzureBlobStorageControllerService : ControllerBase, IFileControllerService
	{
		private readonly IHttpClientFactory _factory;

		private readonly string _connectionString;

		public AzureBlobStorageControllerService(IHttpClientFactory factory, IConfiguration config)
		{
			_factory = factory;
			_connectionString = config.GetConnectionString("AzureBlobStorage");
		}

		public async Task Download(string fileName, string folder)
		{
			throw new NotImplementedException();
		}

		public async Task<IActionResult> Fetch(string fileName, string folder)
		{
			throw new NotImplementedException();
		}

		public async Task<IActionResult> Upload(IFormFile file, string folder)
		{
			string containerName = folder;

			if (file == null || file.Length == 0) { return BadRequest("No file selected"); }
			if (containerName == null) { return BadRequest("No container name supplied"); }
			if (containerName != "media" &&
				containerName != "avatars") { return BadRequest("Invalid container name supplied"); }
			try
			{
				string uniqueBlobName = Guid.NewGuid().ToString() + "_" + file.FileName;


				var container = new BlobContainerClient(_connectionString, containerName);
				container.Create();

				var blob = container.GetBlobClient(uniqueBlobName);
				await blob.UploadAsync(file.OpenReadStream());
				return Ok(uniqueBlobName);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

	}
}
