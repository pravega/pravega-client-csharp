# Project Name
Dell Pravega C# API
## Project summary

### One-sentence description of the project

This project holds a C# library for using the Pravega data management system created by Dell.

### Additional information about the project

Pravega is an open-source storage system implemented and led by Dell Technologies. It uses Streams as a first-class primitive which are based on the append-only log data structure. They are flexible and have good performance. By implementing clients for Pravega in multiple languages, its use can expand to a variety of applications. It currently has clients in Java, Rust, and Python. It does not currently have a client for the .NET framework written in C#. The goal of this project is to create a C# wrapper in the hopes of implementing Pravega in one of the most used frameworks in the world.

### Prerequisites

The user should have the most current version of .NET Core installed on their system. 

### Add-ons

Interoptopus: Interoptopus is a program that was developed to facilitate the wrapping of Rust Code in C#. It is included currently for quick access while developing the project.

Pravega Rust Api: The original Rust code will be present for the wrapper to be able to call to. 

### Installation Steps

Currently, there is only skeleton test code for wrapping Rust code in C#. The installation should be relatively simple, however. The user will git clone the project, and then move into a folder with their source code. Then the user will have to add a reference to the .DLL containing the wrapper.
'git clone https://github.com/WSUCptSCapstone-Fall2022Spring2023/dell-pravegaapi.git'


## Functionality

Currently only test code wrapping Rust in C# has been implemented. The funcitonality of the final product should be no different than native C# code. A user should be able to include the library, and then call functions just like they would any other library. Maintaining standard C# usage is a key priority of this project.


## Known Problems

No known problems as of 11.09.22


## Contributing

1. Fork it!
2. Create your feature branch: `git checkout -b my-new-feature`
3. Commit your changes: `git commit -am 'Add some feature'`
4. Push to the branch: `git push origin my-new-feature`
5. Submit a pull request :D

## Additional Documentation

Sprint Report 1: https://github.com/WSUCptSCapstone-Fall2022Spring2023/dell-pravegaapi/blob/main/Sprint%20Reports/sprint_report_1.md
Sprint Report 2: https://github.com/WSUCptSCapstone-Fall2022Spring2023/dell-pravegaapi/blob/main/Sprint%20Reports/sprint_2_report.md

## License

MIT License

Copyright (c) 10.9.22 John Sbur, Brandon Cook, Samuel Lopez

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
