using GlideBuy.Models;

namespace GlideBuy.Core.Domain.Customers
{
    /**
     * This class enables:
     * Storing multiple passwords per user
     * Supporting password history validation (e.g., "you cannot reuse your last 5 passwords")
     * Tracking when passwords were created
     * Supporting password rotation policies
     * 
     * This class enables a pattern that is not obvious at first:
     * Passwords are append-only, not updated in place
     * 
     * This allows:
     * Keeping old passwords
     * Comparing against previous ones
     * Rolling back (in theory)
     * Auditing changes
     * 
     * Why Support Multiple Formats?
     * This exists for backward compatibility and migration.
     * 
     * Real scenario:
     * Old system stored passwords in plain text
     * New system uses hashing
     * 
     * This allows:
     * Logging in with old format
     * Then upgrading password on next login
     * 
     * This is a migration-friendly design.
     */
    public class CustomerPassword : BaseEntity
    {
        public int CustomerId { get; set; }

        public Customer? Customer { get; set; }

        public string Password { get; set; }

        // For hashing.
        public string PasswordSalt { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public int PasswordFormatId { get; set; }

        public PasswordFormat PasswordFormat
        {
            get => (PasswordFormat)PasswordFormatId;
            set => PasswordFormatId = (int)value;
        }
    }
}
