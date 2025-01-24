using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObenApp.Services
{
    public class PermissionService
    {
        private Dictionary<string, bool> _permissions = new Dictionary<string, bool>();

        public event Action? PermissionsUpdated;

        public void UpdatePermissions(Dictionary<string, bool> newPermissions)
        {
            _permissions = newPermissions;
            PermissionsUpdated?.Invoke(); // Notifica que los permisos han cambiado
        }

        public bool HasPermission(string permission)
        {
            return _permissions.TryGetValue(permission, out var hasPermission) && hasPermission;
        }
    }

}
