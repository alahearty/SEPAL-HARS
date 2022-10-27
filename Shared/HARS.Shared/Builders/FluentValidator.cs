using HARS.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HARS.Shared.Builders
{
    public class FluentValidator
    {
        public FluentValidator()
        {
            Result = ActionResult.Success();
        }
        public FluentValidator IsValidEmail(string email, string error)
        {
            AddCheck(email.IsValidEmailString(), error);
            return this;
        }

        public bool IsValidCollection(object roleNames, string v)
        {
            throw new NotImplementedException();
        }

        public FluentValidator IsValidEmailCollection(IEnumerable<string> emails, string errorMessage = null)
        {
            if (emails == null) return this;

            foreach (var email in emails)
            {
                AddCheck(email.IsValidEmailString(), errorMessage ?? email + " is not a valid email.");
            }
            return this;
        }
        public FluentValidator IsValidText(string text, string error)
        {
            AddCheck(text.IsValid(), error);
            return this;
        }
        public FluentValidator IsNot(object item, object expected, string error)
        {
            AddCheck(item != expected, error);
            return this;
        }
        public FluentValidator IsGreaterThan(double item, double expected, string error)
        {
            AddCheck(item > expected, error);
            return this;
        }
        public FluentValidator IsGreaterThanOrEqualTo(double item, double expected, string error)
        {
            AddCheck(item >= expected, error);
            return this;
        }
        public FluentValidator IsValidCollection<T>(IEnumerable<T> collection, string error)
        {
            AddCheck(collection != null && collection.Count() > 0, error);
            return this;
        }
        public FluentValidator IsValidGuid(Guid guid, string error)
        {
            AddCheck(guid.IsValidGuid(), error);
            return this;
        }
        public FluentValidator IsValidStream(Stream stream, string error)
        {
            AddCheck(stream != null && stream.Length > 0, error);
            return this;
        }
        public FluentValidator IsValidGuidCollection(List<Guid> guids)
        {
            if (guids == null || guids.Count == 0)
            {
                return IsValidCollection(guids, "Ids cannot be null or empty");
            }
            guids.ForEach(x => AddCheck(x.IsValidGuid(), "Invalid Id"));
            return this;
        }
        public FluentValidator IsTrue(bool check, string error)
        {
            AddCheck(check, error);
            return this;
        }
        private void AddCheck(bool passedValidation, string errorMessage)
        {
            if (Result.WasSuccessful && !passedValidation)
            {
                Result = ActionResult.Failed().AddError(errorMessage);
            }
            else if (Result.NotSuccessful && !passedValidation)
            {
                Result.AddError(errorMessage);
            }
        }
        public ActionResult Result { get; private set; }
    }
}
