namespace OnlineStore.Infrastructure.Localization
{
	// // The names come from String.Format(format, args)
	public delegate LocalizedString Localizer(string format, params object[] args);
}
