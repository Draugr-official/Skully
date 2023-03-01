@format = constant [4 x i8] c"%s\0A\00"
@writemode = constant [2 x i8] c"w\00"

; System.Console
define void @Console.WriteLine(i8*, ...) {
entry:
  %1 = call i32 (i8*, ...) @printf(i8* getelementptr inbounds ([4 x i8], [4 x i8]* @format, i32 0, i32 0), i8* %0)
  ret void
}

declare i32 @printf(i8*, ...)

define void @Console.Write(i8*, ...) {
entry:
  %1 = call i32 (i8*, ...) @printf(i8* getelementptr inbounds ([4 x i8], [4 x i8]* @format, i32 0, i32 0), i8* %0)
  ret void
}

; System.File
declare i32 @fopen(i8*, i8*)
declare i32 @fclose(i32)
declare i32 @fwrite(i8*, i32, i32, i32)

define void @File.WriteAllText(i8* %filename, i8* %text) {
  ; Open file in write mode
  %mode = getelementptr inbounds [2 x i8], [2 x i8]* @writemode, i32 0, i32 0
  %file = call i32 @fopen(i8* %filename, i8* %mode)

  ; Get length of text
  %len = call i32 @strlen(i8* %text)

  ; Write text to file
  call i32 @fwrite(i8* %text, i32 1, i32 %len, i32 %file)

  ; Close file
  call i32 @fclose(i32 %file)

  ret void
}

declare i32 @strlen(i8*)