using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Axilis.WP.Demo.ViewModel
{
    public class MainViewModel
    {
        public bool IsDataLoaded
        { get; set; }

        public ObservableCollection<string> ItemsStrings
        { get; private set; }
        public string SelectedString
        { get; set; }

        public ObservableCollection<SimpleKeyValue> ItemsSimpleKeyValue
        { get; private set; }
        public SimpleKeyValue SelectedSimpleKeyValue
        { get; set; }

        public ObservableCollection<Person> ItemsPersons
        { get; private set; }
        public Person SelectedPerson
        { get; set; }

        public Func<string, object> PersonSearch
        { get; private set; }

        public MainViewModel()
        {
            ItemsStrings = new ObservableCollection<string>();
            ItemsSimpleKeyValue = new ObservableCollection<SimpleKeyValue>();
            ItemsPersons = new ObservableCollection<Person>();

            PersonSearch = new Func<string, object>((searchText) =>
            {
                return ItemsPersons.Where(p => p.FullName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) > -1);
            });
        }

        public void LoadData()
        {
            var strings = new List<string>() { "String1", "String2", "String3", "Test1", "Test2", "Test3" };
            var keyValues = new List<SimpleKeyValue>()
            {
                new SimpleKeyValue{ Key = "Key1", Value = "Value1"},
                new SimpleKeyValue{ Key = "Key2", Value = "Value2"},
                new SimpleKeyValue{ Key = "Key3", Value = "Value3"},
                new SimpleKeyValue{ Key = "Key4", Value = "Value4"},
                new SimpleKeyValue{ Key = "Key4", Value = "NewValue5"},
                new SimpleKeyValue{ Key = "Key4", Value = "NewValue6"},
                new SimpleKeyValue{ Key = "Key4", Value = "NewValue7"},
            };
            var persons = new List<Person>()
            {
                new Person{ Id = 1, FirstName = "John", LastName = "Snow" },
                new Person{ Id = 2, FirstName = "Jerry", LastName = "McGuire" },
                new Person{ Id = 3, FirstName = "John", LastName = "Legend" },
                new Person{ Id = 4, FirstName = "Snowman", LastName = "Cooly" },
                new Person{ Id = 5, FirstName = "Donthave", LastName = "Idea" },
                new Person{ Id = 6, FirstName = "Stupid", LastName = "Example" },
                new Person{ Id = 7, FirstName = "Peter", LastName = "Peterovski" },
            };

            ItemsStrings.Clear();
            foreach (var item in strings)
                ItemsStrings.Add(item);

            ItemsSimpleKeyValue.Clear();
            foreach (var item in keyValues)
                ItemsSimpleKeyValue.Add(item);

            ItemsPersons.Clear();
            foreach (var item in persons)
                ItemsPersons.Add(item);

            IsDataLoaded = true;
        }
    }

    public class Person
    {
        public int Id
        { get; set; }
        public string FirstName
        { get; set; }
        public string LastName
        { get; set; }
        public string FullName
        { get { return FirstName + " " + LastName; } }
    }
}
