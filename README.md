# Student Enrolment Management Application

## Introduction

This C# application manages student data using a dictionary, where each student is identified by a unique ID. The application allows users to add, remove, display, and update the enrolment status of students. It also provides functionality to search students by their name, display students with active enrolment, and handle multiple students with the same name.

The application is implemented as a **Console Application** or a **Windows Forms Application** and allows efficient management of students using dictionary operations.

## Table of Contents
- [Introduction](#introduction)
- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
- [Classes and Methods](#classes-and-methods)
- [Examples](#examples)
- [Dependencies](#dependencies)
- [Troubleshooting](#troubleshooting)
- [Contributors](#contributors)
- [License](#license)

## Features
1. **Add a Student**: Add a new student by entering their ID and name. The enrolment status is set to `true` (active) by default.
2. **Remove a Student**: Remove a student from the dictionary by their unique ID.
3. **Display Enrolment Status**: Check the enrolment status (true/false) of a student by providing their ID.
4. **Update Enrolment Status**: Update a student’s enrolment status (active/inactive) by their ID.
5. **Display All Active Students**: Display the IDs and names of all students with an active enrolment status.
6. **Search by Name**: Display the information (ID, enrolment status) of all students with a given name. If multiple students have the same name, display all of them.

## Installation

1. **Clone or Download the Project**:
   - Clone the repository:
     ```bash
     git clone https://github.com/your-username/student-enrolment-management.git
     ```
   - Or download the ZIP file and extract it to a folder on your computer.

2. **Open the Project in Visual Studio**:
   - Open the `.sln` file in Visual Studio.

3. **Build the Solution**:
   - In Visual Studio, click on `Build > Build Solution` or press `Ctrl+Shift+B` to compile the code.

4. **Run the Project**:
   - Press `F5` to run the application in Debug mode.

## Usage

### Console Application
Upon running the console application, you will be presented with a menu offering the following options:
1. Add a new student
2. Remove a student
3. Display a student’s enrolment status
4. Update a student’s enrolment status
5. Display all students with active enrolment
6. Search for students by name

You can input the corresponding number to select the operation you want to perform.

### Example Menu
```
1. Add a New Student
2. Remove a Student
3. Display Enrolment Status of a Student
4. Update Enrolment Status of a Student
5. Display All Active Students
6. Search Students by Name
7. Exit
```

### Input Example
For adding a new student:
- Enter the student's **ID**: `S123`
- Enter the student's **Name**: `John Doe`

For updating enrolment status:
- Enter the **ID** of the student: `S123`
- Update the status: `true` (active) or `false` (inactive)

## Classes and Methods

### 1. `Student` Class
This class stores the details of each student.
- **Fields**:
  - `Name`: The name of the student.
  - `ID`: The unique ID of the student (string).
  - `EnrolmentStatus`: The enrolment status (`true` for active, `false` for inactive).

- **Constructor**: Initializes a new student with a given ID, name, and sets `EnrolmentStatus` to `true` by default.

### 2. `Dictionary<string, Student>` (Data Structure)
A dictionary is used to store students, where the key is the student's ID and the value is the corresponding `Student` object.

### 3. Application Methods
- **AddStudent(string id, string name)**: Adds a new student to the dictionary.
- **RemoveStudent(string id)**: Removes a student from the dictionary using their ID.
- **DisplayEnrolmentStatus(string id)**: Displays the enrolment status of a student by their ID.
- **UpdateEnrolmentStatus(string id, bool status)**: Updates the enrolment status of a student.
- **DisplayActiveStudents()**: Lists all students with `EnrolmentStatus = true`.
- **SearchByName(string name)**: Searches for students by name and displays their ID and enrolment status.

## Examples

### 1. Adding and Removing Students
```csharp
// Add a student
AddStudent("S123", "John Doe");

// Remove a student
RemoveStudent("S123");
```

### 2. Display and Update Enrolment Status
```csharp
// Display enrolment status of a student
DisplayEnrolmentStatus("S123");  // Output: True (default)

// Update enrolment status
UpdateEnrolmentStatus("S123", false);  // Set status to inactive
```

### 3. Display Active Students
```csharp
// Display all active students
DisplayActiveStudents();
```

### 4. Search by Name
```csharp
// Search for all students named "John Doe"
SearchByName("John Doe");
```

## Dependencies
- **C# .NET Core**: Ensure you have the correct version of the .NET Core SDK installed for compiling and running the application.
- **Visual Studio**: This project is designed to be developed and run in Visual Studio.

## Troubleshooting

- **Dictionary Key Error**: Ensure that the student ID entered is unique when adding a new student. The dictionary does not allow duplicate keys.
- **Invalid Input**: Ensure valid inputs are provided when adding, updating, or searching for students (e.g., valid IDs and status values).

## Contributors
- [sijothomas97](https://github.com/sijothomas97)

## License
This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.
