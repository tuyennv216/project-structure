using Core.Authority.Contexts;
using Core.Authority.Metadata;
using Microsoft.EntityFrameworkCore;

namespace Core.Authority.Cases.Permissions
{
	public class PermissionGet
	{
		private readonly AuthorityDb _authorityDb;

		public PermissionGet(AuthorityDb authorityDb)
		{
			this._authorityDb = authorityDb;
		}

		public async Task<string[]> GetAsync(Guid UserId)
		{
			var now = DateTime.UtcNow;

			var validPermissions = await _authorityDb.Permissions
				.AsNoTracking()
				.Where(p => p.UserId == UserId
					&& p.Status == PermissionStatus.Ok
					&& p.DateStart < now
					&& p.DateEnd > now)
				.Select(p => p.TargetName)
				.ToArrayAsync();

			return validPermissions ?? [];
		}
	}
}
