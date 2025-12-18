using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQ.MockData;

namespace LINQ
{
    public class Joins   // Join, GroupJoin, and SelectMany.
    {
        private readonly List<Student> student;
        private readonly List<Teacher> teacher;
        private readonly List<Department> department;
        private readonly List<Address> address;
        public Joins()
        {
            student = new List<Student>
            {
                new Student{ FirstName = "Bill",
                    LastName = "Smith",
                    TeacherID = 1,
                    Year = GradeLevel.FirstYear,
                    Scores = new List<int> { 90, 80, 70, 60 },
                    DepartmentID = 1,
                    AddressId =1
                },
                new Student{ FirstName = "anbu",
                    LastName = "vel",
                    TeacherID = 2,
                    Year = GradeLevel.SecondYear,
                    Scores = new List<int> { 90, 80, 70, 70 },
                    DepartmentID =2 ,
                     AddressId =0
                },
                new Student{ FirstName = "siva",
                    LastName = "sakthi",
                    TeacherID = 3,
                    Year = GradeLevel.ThirdYear,
                    Scores = new List<int> { 90, 80, 70, 100 },
                    DepartmentID = 3,
                     AddressId =0
                },
                 new Student{ FirstName = "valli",
                    LastName = "deivanai",
                    Year = GradeLevel.ThirdYear,
                    Scores = new List<int> { 90, 80, 70, 100 },
                     AddressId =0
                    }
            };
            teacher = new List<Teacher>
            {
                new Teacher{ First = "Sue",
                    Last = "Jones",
                    ID = 1,
                    City = "Seattle" },
                new Teacher{ First = "Bob",
                    Last = "Smith",
                    ID = 2,
                    City = "Tacoma" },
                new Teacher{ First = "Alice",
                    Last = "Johnson",
                    ID = 3,
                    City = "Kirkland" },
            };
            department = new List<Department>
            {
                new Department{ Name = "Computer Science",
                    ID = 1,
                    TeacherID = 1 },
                new Department{ Name = "Accounting",
                    ID = 2,
                    TeacherID = 2 },
                new Department{ Name = "Physics",
                    ID = 3,
                    TeacherID = 3 },
            };

            address = new List<Address>
            {
                new Address{ AddressId = 1,
                    City = "New York",
                    State = "NY" },
                new Address{ AddressId = 2,
                    City = "Los Angeles",
                    State = "CA" },
                new Address{ AddressId = 3,
                    City = "Chicago",
                    State = "IL" },
            };
        }
        #region  join The Join method combines two sequences based on matching keys from each sequence. It returns a new sequence of combined elements.
        public dynamic QuerySyndInnerJoin()
        {
            var query = from s in student
                        join d in department on s.DepartmentID equals d.ID
                        join t in teacher on s.TeacherID equals t.ID
                        select  new { Name = $"{s.FirstName} {s.LastName}", DepartmentName = d.Name, TeacherName = t.First+' '+t.Last};

            foreach (var item in query)
            {
                Console.WriteLine($"{item.Name} - {item.DepartmentName} - {item.TeacherName}");
            }
            return query;
        }
        public dynamic MethodSynInnerJoin() // 
        {
            var query = student.Join(department, s => s.DepartmentID, d => d.ID,
                (s, d) => new { student = s, department = d })
                .Join(teacher, sd => sd.student.TeacherID, t => t.ID,
                (sd, t) => new { sd.student, sd.department, teacher = t })
                .Join(address, sdt => sdt.student.AddressId, a => a.AddressId,
                (sdt, a) => new
                {
                    Name = sdt.student.FirstName + ' ' + sdt.student.LastName,
                    DepartmentName = sdt.department.Name,
                    TeacherName = sdt.teacher.First + ' ' + sdt.teacher.Last,
                    city = a.City
                }).ToList();
                

            foreach (var item in query)
            {
                Console.WriteLine($"{item.Name} -{item.city}- {item.DepartmentName} -{item.TeacherName} ");
            }
            return query;
        }

        public dynamic QueryLeftJoin()  // GroupJoin + SelectMany + DefaultIfEmpty
        {
            var query = from s in student
                        join a in address on s.AddressId equals a.AddressId into GroupStudentAddress
                        from c in GroupStudentAddress.DefaultIfEmpty()
                        select new
                        {
                            Student = s.FirstName +' '+s.LastName,
                            Address = c == null ? "NA" : c.City
                        };
            foreach (var student in query)
            {
                Console.WriteLine($"   {student.Student} {student.Address}");
            }
            return query;
        }
        public dynamic MethodLeftJoin() // GroupJoin + SelectMany + DefaultIfEmpty
        {
            var query = student.GroupJoin(address, s => s.AddressId, a => a.AddressId,
                (student, AddressGroup) => new { student, AddressGroup })
                .SelectMany(a => a.AddressGroup.DefaultIfEmpty(),
                (s, a) => new
                {
                    student = s.student.FirstName + ' ' + s.student.LastName,
                    city = a ==null?"NA":a.City
                });
                
            foreach (var student in query)
            {
                Console.WriteLine($"   {student.student} {student.city}");
            }
            return query;
        }
        public dynamic MethodRightJoin() // Not directly supported in LINQ, but can be simulated using GroupJoin and SelectMany.
        {
            var query = address.GroupJoin(student, s => s.AddressId, a => a.AddressId,
                (a, s) => new { a, s })
                .SelectMany(address => address.s.DefaultIfEmpty(),
                (ad, st) => new
                {
                    City = ad.a.City,
                    StudentName = st == null ? "NULL" : st.FirstName + ' ' + st.LastName
                });
            foreach (var student in query)
            {
                Console.WriteLine($"   {student.StudentName} {student.City}");
            }
            return query;

        }
        public dynamic QueryFullOuterJoin() // Not directly supported in Linq, LeftJoin + Union + RightJoin.
        {
            var leftJoin = from s in student
                           join a in address on s.AddressId equals a.AddressId into LeftStudentAddress
                           from c in LeftStudentAddress.DefaultIfEmpty()
                           select new
                           {
                               studentName = s.FirstName + ' ' + s.LastName,
                               address = c == null ? "NULL" : c.State,
                           };
            var rightjoin = from a in address
                            join s in student on a.AddressId equals s.AddressId into RightStudentAddress
                            from c in RightStudentAddress.DefaultIfEmpty()
                            select new
                            {
                                studentName = c?.FirstName + " " + c?.LastName, //
                                address = a.State,   
                            };
            var FullOuterJoin = leftJoin.Union(rightjoin);
            foreach (var student in FullOuterJoin)
            {
                Console.WriteLine($"   {student.studentName} {student.address}");
            }
            return FullOuterJoin;
        }
        
        public dynamic QueryCrossJoin()
        {
            var query = from s in student
                        from a in address
                        select new
                        {
                            studentName = s.FirstName + ' ' + s.LastName,
                            address = a.City
                        };
            foreach (var student in query)
            {
                Console.WriteLine($"   {student.studentName} {student.address}");
            }
            return query;
        }
        public dynamic MethodCrossJoin() // we achieved through selectmany or join
        {
            var query = student.SelectMany(a => address,
                (s, a) => new
                {
                    studentName = s.FirstName + ' ' + s.LastName,
                    address = a.City
                });

            var query1 = student.Join(address, std => true, a => true,
                (std, a) => new
                {
                    studentName = std.FirstName + ' ' + std.LastName,
                    address = a.City
                });
            foreach (var student in query)
            {
                Console.WriteLine($"   {student.studentName} {student.address}");
            }
            return query;
        }
        #endregion  join

        #region Group join //The GroupJoin method is similar to Join, but it groups elements from the first sequence and associates them with matching elements from the second sequence.
        public void MethodGroupJoin()
        {
            var query = teacher.GroupJoin(student, t => t.ID, s => s.TeacherID,
                (t, studentGroup) => new
                {
                    TeacherName = t.First + " " + t.Last,
                    Students = studentGroup
                });
            foreach (var item in query)
            {
                Console.WriteLine($"{item.TeacherName}");
                foreach (var student in item.Students)
                {
                    Console.WriteLine($"   {student.FirstName} {student.LastName}");
                }
            }
        }
        public void QueryGroupJoin()
        {
            var query = from t in teacher
                        join s in student on t.ID equals s.TeacherID into studentGroup
                        select new
                        {
                            TeacherName = t.First + " " + t.Last,
                            Students = studentGroup
                        };
            foreach (var item in query)
            {
                Console.WriteLine($"{item.TeacherName}");
                foreach (var student in item.Students)
                {
                    Console.WriteLine($"   {student.FirstName} {student.LastName}");
                }
            }
        }
        #endregion Group Join

        #region SelectMany // The SelectMany method flattens(1-o-many) a collection of collections into a single collection. It is often used to project and flatten nested sequences.

        public void SelectManyMethod()
        {
            var query = teacher.SelectMany(t => student.Where(s => s.TeacherID == t.ID),
                (t, s) => new
                {
                    TeacherName = t.First + " " + t.Last,
                    StudentName = s.FirstName + " " + s.LastName
                });
            foreach (var item in query)
            {
                Console.WriteLine($"{item.TeacherName} - {item.StudentName}");
            }
        }

        public void SelectManyQuery()
        {
            var query = from t in teacher
                        from s in student
                        where t.ID == s.TeacherID
                        select new
                        {
                            TeacherName = t.First + " " + t.Last,
                            StudentName = s.FirstName + " " + s.LastName
                        };
            foreach (var item in query)
            {
                Console.WriteLine($"{item.TeacherName} - {item.StudentName}");
            }
        }
        #endregion SelectMany 
    }
}
