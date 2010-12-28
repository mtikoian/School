#pragma once
#include <iostream>

using namespace std;

class CDist
{
	int _iSN;
	int _iEW;

public:	
	CDist(int iSN, int iEW) : _iSN (iSN), _iEW (iEW) {}

	CDist operator+ (CDist const &) const;
	CDist & operator -= (CDist const &);
	CDist operator- () const;
	CDist & operator= (int);
	CDist & operator++ ();
	CDist operator++ (int);

	friend ostream & operator<< (ostream &, CDist const &);
	friend CDist operator- (CDist const & lhs, CDist const & rhs);
	friend bool operator! (CDist const & arg);

	operator double () const;
};

CDist & operator+= (CDist & lhs, CDist const & rhs);