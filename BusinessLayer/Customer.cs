using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhumlaKamnandiProject.BusinessLayer
{
    public class Customer : Person
    {
        #region attributes
        private string customerID;
        private long IDNumber;
        private string accountStatus;
        private string customerAddress;
        #endregion

        #region properties
        public string CustomerID
        {
            get
            {
                return customerID;
            }

            set
            {
                customerID = value;
            }
        }

        public string AccountStatus
        {
            get
            {
                return accountStatus;
            }

            set
            {
                accountStatus = value;
            }
        }

        public string CustomerAddress
        {
            get
            {
                return customerAddress;
            }

            set
            {
                customerAddress = value;
            }
        }

        public long IDNumber1
        {
            get
            {
                return IDNumber;
            }

            set
            {
                IDNumber = value;
            }
        }
        #endregion

        #region constructors
        public Customer()
        {
            customerID = "";
            IDNumber1 = 0;
            accountStatus = "";
            customerAddress = "";
        }

        public Customer(string customerID, int IDNumber, int currentCredit, string creditStatus, string customerAddress)
        {
            this.customerID = customerID;
            this.IDNumber1 = IDNumber;
            this.accountStatus = creditStatus;
            this.customerAddress = customerAddress;
        }
        #endregion

        #region Method
        public override string ToString()
        {
            return this.customerID + ": " + this.Name + " " + this.Surname;
        }
        #endregion
    }
}
