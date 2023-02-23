; ModuleID = 'conditional'
source_filename = "conditional"

@0 = private unnamed_addr constant [4 x i8] c"%s\0A\00"

define void @Console.WriteLine(i8*, ...) {
entry:
  %1 = call i32 (i8*, ...) @printf(i8* getelementptr inbounds ([4 x i8], [4 x i8]* @0, i32 0, i32 0), i8* %0)
  ret void
}

declare i32 @printf(i8*, ...)

define void @Console.Write(i8*, ...) {
entry:
  %1 = call i32 (i8*, ...) @printf(i8* getelementptr inbounds ([4 x i8], [4 x i8]* @0, i32 0, i32 0), i8* %0)
  ret void
}