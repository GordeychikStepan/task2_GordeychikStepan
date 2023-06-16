using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
	static class PhoneBookLoader
	{
		public static void Load(PhoneBook phoneBook, string fileName)
		{
			if (!File.Exists(fileName)) { MessageBox.Show("Файл не найден.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information); }

			var lines = File.ReadAllLines(fileName);

			foreach (var line in lines)
			{
				var parts = line.Split(';');
				if (parts.Length == 2) { phoneBook.AddContact(new Contact { Name = parts[0].Trim(), Phone = parts[1].Trim() }); }
			}
		}

		public static void Save(PhoneBook phoneBook, string fileName)
		{
			var lines = phoneBook.GetContacts()
				.Select(c => c.Name + ";" + c.Phone)
				.ToArray();

			File.WriteAllLines(fileName, lines);
		}
	}
}
