// FileSearcher.cpp : ���� DLL Ӧ�ó���ĵ���������
//

#include "stdafx.h"
#include "FileSearcher.h"


// ���ǵ���������һ��ʾ��
FILESEARCHER_API int nFileSearcher=0;

// ���ǵ���������һ��ʾ����
FILESEARCHER_API int fnFileSearcher(void)
{
	return 42;
}

// �����ѵ�����Ĺ��캯����
// �й��ඨ�����Ϣ������� FileSearcher.h
CFileSearcher::CFileSearcher()
{
	return;
}
