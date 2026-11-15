\# FileSorter



FileSorter is a C# console application for analysing files in a directory and suggesting organised destination folders based on file type.



The project is being developed incrementally, with each version adding functionality while keeping individual components separated and testable.



\## Current Version



\*\*v0.1 — Dry-Run MVP\*\*



The current version can:



\- Accept a source directory from the user.

\- Scan files directly contained within that directory.

\- Analyse each file based on its extension.

\- Classify files into predefined file types.

\- Generate a suggested destination directory for each file.

\- Display the proposed source and destination paths.



v0.1 does \*\*not move or modify any files\*\*.



The purpose of this version is to establish and test the core application structure before filesystem modifications are introduced.



\## Current File Categories



Files are currently classified into:



\- Images

\- Videos

\- Audio

\- Books

\- Documents

\- Other



Classification is currently based entirely on file extensions. Unsupported extensions are classified as `Other`.



\## Architecture



The current processing pipeline is:



```text

Source Directory

&#x20;     |

&#x20;     v

&#x20;  Scanner

&#x20;     |

&#x20;     v

&#x20;  Analyzer

&#x20;     |

&#x20;     v

SortingSuggester

&#x20;     |

&#x20;     v

SuggestedMove

&#x20;     |

&#x20;     v

Dry-Run Console Output

```



\### Scanner



Retrieves the full paths of files directly contained within the selected source directory.



Directory scanning is currently non-recursive.



\### Analyzer



Examines an individual file path, extracts its extension, and assigns a `FileType`.



\### SortingSuggester



Uses the analysed file type and a configured destination root to determine the proposed destination directory.



It does not create directories or move files.



\### SuggestedMove



Represents a proposed file operation by storing:



\- The original `AnalyzedFile`.

\- The suggested destination directory.



\## Example



Given:



```text

Source:

C:\\Users\\User\\Downloads\\photo.jpg



Destination root:

D:\\Sorted Files

```



FileSorter can produce:



```text

C:\\Users\\User\\Downloads\\photo.jpg

\-> D:\\Sorted Files\\Images

```



No file is moved in v0.1.



\## Testing



The core components are covered by automated tests using xUnit.



Tests currently cover:



\- File extension classification.

\- Unknown file types.

\- Empty directories.

\- Invalid source directories.

\- Non-recursive directory scanning.

\- File type to destination-folder mapping.

\- Preservation of the analysed file when creating a suggested move.



Filesystem tests use isolated temporary directories to avoid depending on machine-specific test data.



\## Running the Project



Requirements:



\- .NET 10 SDK



From the repository root:



```bash

dotnet build

dotnet test

dotnet run --project src/FileSorter

```



The application will ask for:



1\. The directory to scan.

2\. The destination root to use when generating suggestions.



\## Limitations of v0.1



The current version intentionally does not include:



\- Actual file movement.

\- Collision handling.

\- Recursive directory scanning.

\- Input validation.

\- Configuration files.

\- Logging.

\- Undo functionality.

\- Advanced file identification beyond extension-based classification.



These are candidates for later versions.



\## Planned Development



\### v0.2



The next development stage will focus on safely performing file operations.



Planned areas include:



\- A dedicated file-moving component.

\- Destination directory creation.

\- Filename collision handling.

\- Automated tests for file movement.

\- Maintaining a dry-run mode before executing changes.



Additional classification rules and usability improvements will be introduced in later versions as the core application develops.



\## Project Status



v0.1 MVP complete.



The application currently has a working end-to-end dry-run pipeline:



```text

Scanner -> Analyzer -> SortingSuggester -> SuggestedMove

```

