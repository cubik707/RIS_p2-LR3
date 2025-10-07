using System;

namespace RIS_p2_LR3
{
    public class DepartmentJson
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int EmployeeCount { get; set; }
        public string CreatedDate { get; set; }
        public string LastModifiedDate { get; set; }

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
