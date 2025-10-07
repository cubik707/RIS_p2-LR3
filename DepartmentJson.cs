using System;

namespace RIS_p2_LR3
{
    /// <summary>
    /// JSON representation of Department for serialization/deserialization
    /// </summary>
    public class DepartmentJson
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int EmployeeCount { get; set; }
        public string CreatedDate { get; set; }
        public string LastModifiedDate { get; set; }

        /// <summary>
        /// Converts DepartmentJson to Department
        /// </summary>
        /// <returns>Department object</returns>
        public Department ToDepartment()
        {
            return new Department
            {
                Id = Id,
                Name = Name,
                Description = Description,
                EmployeeCount = EmployeeCount,
                CreatedDate = DateTime.Parse(CreatedDate),
                LastModifiedDate = DateTime.Parse(LastModifiedDate)
            };
        }

        /// <summary>
        /// Creates DepartmentJson from Department
        /// </summary>
        /// <param name="department">Department object</param>
        /// <returns>DepartmentJson object</returns>
        public static DepartmentJson FromDepartment(Department department)
        {
            return new DepartmentJson
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description,
                EmployeeCount = department.EmployeeCount,
                CreatedDate = department.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss"),
                LastModifiedDate = department.LastModifiedDate.ToString("yyyy-MM-dd HH:mm:ss")
            };
        }
    }
}
