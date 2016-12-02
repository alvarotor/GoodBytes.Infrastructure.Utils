namespace GoodBytes.Infrastructure.Utils.Models
{
	public class ContactsModel
	{
		public string Email { get; set; }
		public string Name { get; set; }
		public bool HasName
		{
			get
			{
				return !string.IsNullOrEmpty(Name);
			}
		}

		public bool Selected
		{
			get { return true; }
		}
	}
}
