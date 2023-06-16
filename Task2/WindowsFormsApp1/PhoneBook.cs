using System;
using System.Collections.Generic;
using System.Linq;

namespace WindowsFormsApp1
{
	public class PhoneBook
	{
		private List<Contact> contacts;

		public PhoneBook()
		{
			contacts = new List<Contact>();
		}

		public void AddContact(Contact contact) { contacts.Add(contact); }

		public void RemoveContact(string name)
		{
			Contact contactToRemove = contacts.FirstOrDefault(c => c.Name == name);
			if (contactToRemove != null) { contacts.Remove(contactToRemove); }
		}

		public List<Contact> GetContacts() { return contacts; }

		public List<Contact> SearchContacts(string searchName) 
		{ 
			return contacts.Where(c => c.Name.ToLower().Contains(searchName)).ToList();
		}
	}
}
