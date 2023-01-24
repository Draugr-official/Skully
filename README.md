<p align="center">
  <img alt="Skully logo" src="https://media.discordapp.net/attachments/671694363235057675/1067004440554512454/image.png">
</p>
<h1 align="center">The official Skully compiler</h1>
<p align="center">
  <img src="https://github.com/Draugr-official/Skully/actions/workflows/dotnet.yml/badge.svg">
  <img src="https://img.shields.io/badge/version-0.0.1-blue">
</p>

<h2>Introduction</h2>
<p>Skully is a user driven compiler dedicated to producing high performance, compact and self-contained software in C#.</p>

<h2>Get started</h2>
<p>
  To start using Skully, make sure you have Clang LLVM installed.
  <ol>
    <li>Download LLVM: https://github.com/llvm/llvm-project/releases</li>
    <li>Find the latest version of LLVM and download LLVM-XX.X.X-win64.exe</li>
    <li>Install it to <code>C:\Program Files\LLVM</code></li>
  </ol>
</p>

<h2>Hello, World!</h2>
<p>Every time we learn a new language or technology, we need a start project to understand what we're dealing with. We will start off with the classic 'Hello world' project to learn and understand how Skully works and how you can easily integrate it into your own projects.
<br>
<br>
<strong>Simple</strong> is a word easily associated with Skully. We are no believers in unecessarily complex technology and have decided to keep Skully as simple as possible. Don't be fooled by this, Skully is still packed with features you can easily access whenever you wish.
<br>
<br>
Now, lets set up your Skully environment. Navigate to the <a href="#get-started">Get started</a> section and make sure you have every depdendency necessary installed. Skully will NOT work without these!
<br>
<br>
With that out of the way, lets get started!
<br>
Open your console, navigate to a directory where you want to host your Skully project and enter <code>mkdir Projects</code>. This is to keep track of where your projects are, and to keep them sorted. If you wish to store your project alongside your other C# projects, this works too.
<br>
<br>
To create a new project, enter <code>skully new [name]</code>. This will set up a development environment for you. Next, enter <code>cd [name]</code> to navigate into your newly created project. From here, it works like a normal Visual Studio project, double-click [name].sln to open your project or open it in Visual Studio Code. KEEP IN MIND, to build your project, navigate to your project directory in the console and enter <code>skully build</code> to build your project. This will create a folder under <code>[name]/[name]/bin/debug</code> where you can find your .exe file.
</p>
