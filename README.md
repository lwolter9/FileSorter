# FileSorter

FileSorter is a C# console application for organising files into categorised destination folders based on file type.

The project is being developed incrementally, with each version adding functionality while keeping individual components separated and testable.

## Current Version

**v0.2 — Safe File Moving**

FileSorter can now:

- Accept a source directory from the user.
- Scan files directly contained within that directory.
- Analyse files based on their extensions.
- Classify files into predefined file types.
- Generate destination directories based on those classifications.
- Preview all proposed moves before modifying the filesystem.
- Require user confirmation before executing moves.
- Automatically create missing destination directories.
- Move files into their suggested locations.
- Resolve filename collisions without overwriting existing files.
- Continue processing if an individual file move fails.
- Report successful moves, renamed files, and failed moves at the end of the operation.

The goal of v0.2 is to extend the dry-run architecture introduced in v0.1 into a safe, functional file-moving workflow.

## File Categories

Files are currently classified into:

- Images
- Videos
- Audio
- Books
- Documents
- Other

Classification is currently based entirely on file extensions.

Unsupported or unidentified extensions are classified as `Other`.

## How It Works

The current processing pipeline is:

```text
Source Directory
      |
      v
   Scanner
      |
      v
   Analyzer
      |
      v
SortingSuggester
      |
      v
List<SuggestedMove>
      |
      v
    Preview
      |
      v
User Confirmation
      |
      v
  FileMover
      |
      v
List<MoveResult>
      |
      v
Final Summary
```

The application first generates the complete set of proposed moves.

No files are modified until the user has reviewed the preview and explicitly confirmed that the operation should proceed.

## Architecture

### Scanner

Retrieves the full paths of files directly contained within the selected source directory.

Directory scanning is currently non-recursive.

### Analyzer

Examines an individual file path, extracts its extension, and assigns a `FileType`.

### AnalyzedFile

Represents the result of analysing a file.

It stores information including:

- Full source path.
- File extension.
- Assigned `FileType`.

### SortingSuggester

Uses an `AnalyzedFile` and a configured destination root to determine the appropriate destination directory.

For example:

```text
Destination root:
D:\Sorted Files

File type:
Image

Suggested destination:
D:\Sorted Files\Images
```

`SortingSuggester` determines where a file should go but does not modify the filesystem.

### SuggestedMove

Represents a proposed file operation.

It stores:

- The original `AnalyzedFile`.
- The suggested destination directory.

A collection of `SuggestedMove` objects is generated before any files are moved, allowing the application to display a complete preview.

### FileMover

Executes an approved `SuggestedMove`.

Its responsibilities include:

- Creating the destination directory if it does not exist.
- Constructing the final destination file path.
- Moving the file.
- Detecting filename collisions.
- Resolving collisions without overwriting existing files.
- Converting filesystem failures into a `MoveResult` rather than stopping the entire batch.

### MoveResult

Represents the result of an attempted file move.

It stores:

- Source path.
- Final destination path.
- Whether the move succeeded.
- An error message when the move fails.

It also determines whether a file was renamed during the move by comparing the original and final filenames.

## Filename Collision Handling

FileSorter never intentionally overwrites an existing file.

If the destination already contains a file with the same name, the incoming file is automatically assigned the first available numbered filename.

For example:

```text
photo.jpg
photo(1).jpg
photo(2).jpg
photo(3).jpg
```

If the destination already contains:

```text
photo.jpg
photo(1).jpg
photo(2).jpg
```

the incoming file will be moved as:

```text
photo(3).jpg
```

Collision renames are included in the final operation summary.

## Preview and Confirmation

Before moving anything, FileSorter displays the proposed operations.

Example:

```text
C:\Users\User\Downloads\photo.jpg
-> D:\Sorted Files\Images

C:\Users\User\Downloads\report.pdf
-> D:\Sorted Files\Documents
```

The user is then asked to confirm whether the moves should proceed.

If the operation is cancelled, no files are moved.

## Failure Handling

An individual failed move does not terminate the entire sorting operation.

If a file cannot be moved, FileSorter records the failure and continues processing the remaining files.

At the end of the operation, failures are reported along with their associated error messages.

Example:

```text
Sorting complete.

Moved successfully: 12
Renamed due to collisions: 2
Failed: 1

Renamed Files:
photo.jpg -> photo(1).jpg
report.pdf -> report(2).pdf

Failed moves:
locked.txt
Reason: The process cannot access the file because it is being used by another process.
```

## Testing

The core components are covered by automated tests using xUnit.

Tests currently cover:

- File extension classification.
- Unknown file types.
- Empty directories.
- Invalid source directories.
- Non-recursive directory scanning.
- File type to destination-folder mapping.
- Preservation of analysed files when generating suggestions.
- Successful file movement.
- Automatic destination-directory creation.
- Removal of the source file after a successful move.
- Single filename collisions.
- Multiple sequential filename collisions.
- Preservation of existing files during collisions.
- Correct collision-renaming behaviour.
- Failed filesystem operations.
- Preservation of source files after failed moves.
- Failed `MoveResult` generation rather than unhandled exceptions.

Filesystem tests use isolated temporary directories and files to avoid depending on machine-specific test data.

### End-to-End Testing

v0.2 has also been manually tested against the complete application workflow.

The following scenarios have been verified:

1. Cancelling at the preview stage leaves all files untouched.
2. Confirmed moves place files into the expected category directories.
3. Filename collisions result in automatic renaming without overwriting existing files.
4. Individual move failures are reported while remaining files continue to be processed.

## Running the Project

### Requirements

- .NET 10 SDK

From the repository root:

```bash
dotnet build
dotnet test
dotnet run --project src/FileSorter
```

The application will ask for:

1. The source directory to scan.
2. The destination root for organised files.
3. Confirmation after displaying the proposed moves.

## Example

Given a source directory containing:

```text
Downloads/
├── photo.jpg
├── report.pdf
├── song.mp3
└── installer.exe
```

and the destination root:

```text
D:\Sorted Files
```

FileSorter will suggest destinations such as:

```text
photo.jpg
-> D:\Sorted Files\Images

report.pdf
-> D:\Sorted Files\Documents

song.mp3
-> D:\Sorted Files\Audio

installer.exe
-> D:\Sorted Files\Other
```

After confirmation, the required category directories are created automatically and the files are moved.

## Current Limitations

v0.2 intentionally remains focused on the core sorting workflow.

The current version does not yet include:

- Recursive directory scanning.
- User-defined sorting rules.
- User-defined category structures.
- Manual editing of individual suggestions before execution.
- Metadata-based classification.
- Duplicate detection.
- Content-based file identification.
- Content-based automatic filename generation.
- Undo functionality.
- Persistent configuration.
- Detailed logging.
- Full user-input validation.

These features are candidates for future development rather than requirements of the current version.

## Development History

### v0.1 — Dry-Run MVP

Established the core architecture:

```text
Scanner -> Analyzer -> SortingSuggester -> SuggestedMove
```

v0.1 could scan, classify, and suggest destinations but deliberately made no filesystem modifications.

### v0.2 — Safe File Moving

Extended the v0.1 pipeline with:

- Batch generation of proposed moves.
- Preview before execution.
- User confirmation.
- Actual file movement.
- Automatic destination-directory creation.
- Collision-safe filename generation.
- Per-file result tracking.
- Failure continuation.
- End-of-run reporting.

## Future Direction

The longer-term goal is for FileSorter to become a flexible file-organisation tool rather than relying permanently on a fixed set of extension-based categories.

Potential future features include:

- User-defined folder structures.
- Custom sorting rules.
- Manual override of suggested destinations before execution.
- Sorting based on file metadata.
- Duplicate and similar-file detection.
- Optional removal of confirmed duplicate files.
- Content-based identification of unidentified files.
- Automatic filename suggestions based on file contents.

These features will be introduced incrementally so that each development stage remains manageable and testable.

## Project Status

**v0.2 functional core complete.**

The application currently provides a tested end-to-end workflow:

```text
Scan
-> Analyze
-> Suggest
-> Preview
-> Confirm
-> Move
-> Report
```