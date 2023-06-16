using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
	public partial class Form1 : Form
	{
		private PhoneBook phoneBook;

		public Form1()
		{
			InitializeComponent();
			phoneBook = new PhoneBook();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			LoadPhoneBook();
			UpdateContactList();
		}

		private void LoadPhoneBook()
		{
			try { PhoneBookLoader.Load(phoneBook, "contacts.txt"); } 
			catch (Exception ex) { MessageBox.Show("Ошибка при загрузке с файла: " + ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information); }
		}

		private void SavePhoneBook()
		{
			try { PhoneBookLoader.Save(phoneBook, "contacts.txt"); } 
			catch (Exception ex) { MessageBox.Show("Ошибка при сохранении телефонной книги: " + ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information); }
		}

		private void UpdateContactList()
		{
			contactsDataGridView.Rows.Clear();
			foreach (Contact contact in phoneBook.GetContacts())
			{
				contactsDataGridView.Rows.Add(contact.Name, contact.Phone);
			}
		}

		private void showAllButton_Click(object sender, EventArgs e) { UpdateContactList(); }

		private void searchTextBox_TextChanged (object sender, EventArgs e)
		{
			string searchName = searchTextBox.Text.Trim();

			if (!string.IsNullOrEmpty(searchName))
			{
				List<Contact> searchResults = phoneBook.SearchContacts(searchName);
				contactsDataGridView.Rows.Clear();
				foreach (Contact contact in searchResults)
				{
					contactsDataGridView.Rows.Add(contact.Name, contact.Phone);
				}
			} 
			else UpdateContactList();
		}

		private void addButton_Click (object sender, EventArgs e)
		{
			string name = nameTextBox.Text.Trim();
			string phone = phoneTextBox.Text.Trim();

			if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(phone))
			{
				if (name.Length >= 2 && name.Length < 25)
				{
					if (IsValidPhoneNumber(phone))
					{
						phoneBook.AddContact(new Contact { Name = name, Phone = phone });
						UpdateContactList();
						nameTextBox.Clear();
						phoneTextBox.Clear();
						MessageBox.Show("Контакт был добавлен.", "Отлично!");
					} 
					else MessageBox.Show("Неверный формат номера телефона. Пожалуйста, введите верный формат в виде (XXX)XXX-XX-XX.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
				} 
				else MessageBox.Show("Имя должно содержать как минимум 2 символа и максимум 25.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
			} 
			else MessageBox.Show("Пожалуйста, введите имя и номер телефона.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private bool IsValidPhoneNumber (string phoneNumber)
		{
			string pattern = @"^\(\d{3}\)\d{3}-\d{2}-\d{2}$";
			return Regex.IsMatch(phoneNumber, pattern);
		}

		private void removeButton_Click (object sender, EventArgs e)
		{
			if (contactsDataGridView.SelectedRows.Count > 0)
			{
				DataGridViewRow selectedRow = contactsDataGridView.SelectedRows[0];
				string name = selectedRow.Cells[0].Value.ToString();
				phoneBook.RemoveContact(name);
				UpdateContactList();
			} 
			else MessageBox.Show("Пожалуйста, выберите контакт для удаления.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void Form1_FormClosing (object sender, FormClosingEventArgs e) { SavePhoneBook(); }

		private void saveButton_Click (object sender, EventArgs e) 
		{ 
			SavePhoneBook();
			MessageBox.Show("Список контактов был сохранен.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void button2_Click (object sender, EventArgs e) { Application.Exit(); }

		private void contactsDataGridView_CellClick (object sender, DataGridViewCellEventArgs e)
		{
			if (contactsDataGridView.CurrentRow != null && contactsDataGridView.CurrentRow.Index == contactsDataGridView.Rows.Count - 1 && contactsDataGridView.Rows[contactsDataGridView.CurrentRow.Index].IsNewRow)
			{
				contactsDataGridView.ClearSelection();
			}
		}

		private void Form1_Click (object sender, EventArgs e) { contactsDataGridView.ClearSelection(); }
	}
}
