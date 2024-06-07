using PhumlaKamnandiProject.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhumlaKamnandiProject.BusinessLayer
{
    public class Employee : Person
    {
        #region attributes
        private string employeeID;
        private Role roleValue;

        public enum Role
        {
            noRole = 0,
            receptionist = 1
            

        }
        #endregion

        #region properties
        public string EmployeeID
        {
            get
            {
                return employeeID;
            }

            set
            {
                employeeID = value;
            }
        }

        public Role RoleValue
        {
            get
            {
                return roleValue;
            }

            set
            {
                roleValue = value;
            }
        }

        #endregion

        #region constructors
        public Employee()
        {
            employeeID = "";
            roleValue = Role.noRole;
        }
        #endregion

        #region Method
        public override string ToString()
        {
            return this.employeeID;  // return employee username
        }
        #endregion
    }
}
