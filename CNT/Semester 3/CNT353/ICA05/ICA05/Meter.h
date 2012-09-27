#pragma once
#include <iostream>

using namespace std;

char const gkcBlock = 221;
int const gkiScreenWidth = 78;

class CMeter
{
private:
	enum ERange {ePos, eMin, eMax};
	int * _piRange;
	int Convert () const;
public:
	CMeter (int iCurrent = 0, int iMin = 0, int iMax = 99);
	CMeter (CMeter const & copy);
	~CMeter ();
	void Step ();
	void Reset ();
	void Display (ostream & out = cout) const;
};