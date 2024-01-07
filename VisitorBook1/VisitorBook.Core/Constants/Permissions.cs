using VisitorBook.Core.Enums;

namespace VisitorBook.Core.Constants
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionsList(string module)
        {
            return new List<string>()
            {
                $"Permissions.{module}.View",
                $"Permissions.{module}.Create",
                $"Permissions.{module}.Edit",
                $"Permissions.{module}.Delete"
            };
        }

        public static List<string> GenerateAllPermissions()
        {
            var allPermissions = new List<string>();

            var modules = Enum.GetValues(typeof(Modules));

            foreach (var module in modules)
            {
                allPermissions.AddRange(GeneratePermissionsList(module.ToString()));
            } 

            return allPermissions;
        }

        public static class UserManagement
        {
            public const string View = "Permissions.UserManagement.View";
            public const string Create = "Permissions.UserManagement.Create";
            public const string Edit = "Permissions.UserManagement.Edit";
            public const string Delete = "Permissions.UserManagement.Delete";
        }

        public static class PlaceManagement
        {
            public const string View = "Permissions.PlaceManagement.View";
            public const string Create = "Permissions.PlaceManagement.Create";
            public const string Edit = "Permissions.PlaceManagement.Edit";
            public const string Delete = "Permissions.PlaceManagement.Delete";
        }

        public static class VisitedCountyManagement
        {
            public const string View = "Permissions.VisitedCountyManagement.View";
            public const string Create = "Permissions.VisitedCountyManagement.Create";
            public const string Edit = "Permissions.VisitedCountyManagement.Edit";
            public const string Delete = "Permissions.VisitedCountyManagement.Delete";
        }

        public static class FakeDataManagement
        {
            public const string View = "Permissions.FakeDataManagement.View";
            public const string Create = "Permissions.FakeDataManagement.Create";
            public const string Edit = "Permissions.FakeDataManagement.Edit";
            public const string Delete = "Permissions.FakeDataManagement.Delete";
        }

        public static class ContactMessageManagement
        {
            public const string View = "Permissions.ContactMessageManagement.View";
            public const string Create = "Permissions.ContactMessageManagement.Create";
            public const string Edit = "Permissions.ContactMessageManagement.Edit";
            public const string Delete = "Permissions.ContactMessageManagement.Delete";
        }

        public static class AuditTrailManagement
        {
            public const string View = "Permissions.AuditTrailManagement.View";
            public const string Create = "Permissions.AuditTrailManagement.Create";
            public const string Edit = "Permissions.AuditTrailManagement.Edit";
            public const string Delete = "Permissions.AuditTrailManagement.Delete";
        }

        public static class RegisterApplicationManagement
        {
            public const string View = "Permissions.RegisterApplicationManagement.View";
            public const string Create = "Permissions.RegisterApplicationManagement.Create";
            public const string Edit = "Permissions.RegisterApplicationManagement.Edit";
            public const string Delete = "Permissions.RegisterApplicationManagement.Delete";
        }
    }
}
