using MonoDevelop.Core;

namespace MonoDevelop.VersionControl
{
	public abstract class VersionControlSystem
	{
		/// <summary>
		/// Creates an instance of a repository for this version control system
		/// </summary>
		/// <returns>
		/// The repository instance.
		/// </returns>
		public Repository CreateRepositoryInstance ()
		{
			Repository rep = OnCreateRepositoryInstance ();
			rep.VersionControlSystem = this;
			return rep;
		}

		/// <summary>
		/// Identifier of the version control system
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		/// <remarks>
		/// This identifier is stored in configuration files, so it should not change.
		/// The default implementation returns the full name of the class.
		/// </remarks>
		public virtual string Id {
			get { return GetType().ToString(); }
		}
		
		/// <summary>
		/// Display name of the version control system
		/// </summary>
		public abstract string Name { get; }

		/// <summary>
		/// Gets a value indicating whether this version control system is available
		/// </summary>
		/// <remarks>
		/// If the version control system depends on some native tools or libraries, this method should
		/// check if those dependencies are properly installed and return <c>false</c> if they are not.
		/// </remarks>
		public virtual bool IsInstalled {
			get { return false; }
		}
		
		/// <summary>
		/// Creates an instance of a repository for this version control system
		/// </summary>
		protected abstract Repository OnCreateRepositoryInstance ();
		
		/// <summary>
		/// Creates an editor object for a repository.
		/// </summary>
		/// <returns>
		/// The repository editor.
		/// </returns>
		/// <param name='repo'>
		/// A repository
		/// </param>
		public abstract IRepositoryEditor CreateRepositoryEditor (Repository repo);
		
		/// <summary>
		/// Gets a repository for a given local path and identifier
		/// </summary>
		/// <returns>
		/// The repository.
		/// </returns>
		/// <param name='path'>
		/// A local path
		/// </param>
		/// <param name='id'>
		/// An identifier. This identifier is generated by MD and normally identifies
		/// a project.
		/// </param>
		/// <remarks>
		/// If the local path belongs to a repository that has already returned
		/// in previous calls, the same repository instance should be returned
		/// to optimize memory and resource use. MonoDevelop keeps track of
		/// repository references and will Dispose the repository only when
		/// the last reference to the repo is freed.
		/// </remarks>
		public virtual Repository GetRepositoryReference (FilePath path, string id)
		{
			return VersionControlService.InternalGetRepositoryReference (path, id);
		}
		
		/// <summary>
		/// Currently unused
		/// </summary>
		public virtual void StoreRepositoryReference (Repository repo, FilePath path, string id)
		{
			VersionControlService.InternalStoreRepositoryReference (repo, path, id);
		}
	}
}
