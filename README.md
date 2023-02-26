<p align="center">
  <img alt="Skully logo" src="https://media.discordapp.net/attachments/671694363235057675/1067753089328754698/image.png">
</p>
<h1 align="center">The official Skully compiler</h1>
<p align="center">
  <img src="https://github.com/Draugr-official/Skully/actions/workflows/dotnet.yml/badge.svg">
  <img src="https://img.shields.io/badge/version-0.0.2-blue">
</p>

<h2>Introduction</h2>
<p>Skully is a user driven compiler dedicated to producing high performance, compact and self-contained software in C#.</p>

## Performance
To gather this data, an executeable generated by each compiler is ran 100 times with a stopwatch to measure the time.
All executeables are compiled as their release/most optimized variant.

|Language|Code|Elapsed time(avg)|
|--|--|--|
| C#(Skully) | ```Console.WriteLine("Hello, World!");``` | 565ms |
| Rust(rustc) | ```println!("Hello, World!");``` | 625ms |
| Cpp(gcc) | ```std::cout << "Hello, World!\n";``` | 642ms |

Sample size: 50 sets of 100 runs.
