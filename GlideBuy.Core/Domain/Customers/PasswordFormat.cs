namespace GlideBuy.Core.Domain.Customers
{
    public enum PasswordFormat
    {
        // Plain text (legacy)
        Clear = 0,

        // One-way hashing
        Hashed = 1,

        // Reversible encryption
        Encrypted = 2,
    }
}
