using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyClassLibrary.Interfaces
{
	/// <summary>
	/// And interface for a service that handles file uploads and downloads
	/// </summary>
	public interface IFileControllerService
	{
		/// <summary>
		/// Uploads a file asynchronously to the specified folder. Returns a unique file name as string.
		/// </summary>
		/// <param name="file"></param>
		/// <param name="folder"></param>
		/// <returns>
		/// A unique file name as string.
		/// </returns>
		Task<IActionResult> Upload(IFormFile file, string folder);

		/// <summary>
		/// Downloads a file asynchronously from the specified folder
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="folder"></param>
		/// <returns>
		/// 
		/// </returns>
		Task Download(string fileName, string folder);

		/// <summary>
		/// Fetches a file asynchronously from the specified folder
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="folder"></param>
		/// <returns>
		/// returns a file as a FileContentResult
		/// </returns>
		Task<IActionResult> Fetch(string fileName, string folder);

	}
}
