// ���� ifdef ���Ǵ���ʹ�� DLL �������򵥵�
// ��ı�׼�������� DLL �е������ļ��������������϶���� FILESEARCHER_EXPORTS
// ���ű���ġ���ʹ�ô� DLL ��
// �κ�������Ŀ�ϲ�Ӧ����˷��š�������Դ�ļ��а������ļ����κ�������Ŀ���Ὣ
// FILESEARCHER_API ������Ϊ�Ǵ� DLL ����ģ����� DLL ���ô˺궨���
// ������Ϊ�Ǳ������ġ�
#ifdef FILESEARCHER_EXPORTS
#define FILESEARCHER_API __declspec(dllexport)
#else
#define FILESEARCHER_API __declspec(dllimport)
#endif

// �����Ǵ� FileSearcher.dll ������
class FILESEARCHER_API CFileSearcher {
public:
	CFileSearcher(void);
	// TODO:  �ڴ�������ķ�����
};

extern FILESEARCHER_API int nFileSearcher;

FILESEARCHER_API int fnFileSearcher(void);
