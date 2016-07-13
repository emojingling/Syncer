// FileSearcher.cpp : 定义 DLL 应用程序的导出函数。
//

#include "stdafx.h"
#include "FileSearcher.h"


// 这是导出变量的一个示例
FILESEARCHER_API int nFileSearcher=0;

// 这是导出函数的一个示例。
FILESEARCHER_API int fnFileSearcher(void)
{
	return 42;
}

// 这是已导出类的构造函数。
// 有关类定义的信息，请参阅 FileSearcher.h
CFileSearcher::CFileSearcher()
{
	return;
}
