using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhumlaKamnandiProject.BusinessLayer
{
    public abstract class Person
    {
        #region attributes
        private string name;
        private string surname;
        private string phone;
        #endregion

        #region Properties
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public string Phone
        {
            get
            {
                return phone;
            }
            set
            {
                phone = value;
            }
        }

        public string Surname
        {
            get
            {
                return surname;
            }

            set
            {
                surname = value;
            }
        }
        #endregion

        #region Constructors
        public Person()
        {
            name = "";
            surname = "";
            phone = "";
        }

        public Person(string nameVal, string surnameVal, string phoneVal)
        {
            name = nameVal;
            surname = surnameVal;
            phone = phoneVal;
        }
        #endregion

        #region method
        public override string ToString()
        {
            return name + '\n' + phone;
        }

        #endregion
    }
}
