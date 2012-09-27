				.title by Simon Walker
;**************************************************************************
;* Sample.asm                                                             *
;*                                                                        *
;* The purpose of this program is to demonstrate source file page layout  *
;* and basic AS6811 assembler syntax.                                     *
;**************************************************************************

;.------------------------------------------------------------------------.
;|                           EQUATES SECTION                              |
;`------------------------------------------------------------------------'

LED			=	0xC000			; 2-digit LED display.
RAMSTART	=	0x1040			; first byte of large RAM block
RAMEND		=	0x7FFF			; end of RAM
RESETVEC	=	0xFFFE			; reset vector for normal modes
ROMSTART	=	0xC000			; Lowest EPROM address.
ROMEND		=	0xFFFF			; Highest EPROM address.
STACKTOP	=	0x0FFF			; top of stack (0x0FFF - 0x0100)
PORTA		=	0x1000			; porta data
PACTL		=	0x1026			; portA control

;.------------------------------------------------------------------------.
;|                           TARGET CONTROL                               |
;`------------------------------------------------------------------------'

; set the following variable to 1 to place program in EEPROM
; set the following variable to 0 to place program in RAM

TARGETROM = 0		; 0 == RAM, 1 == EEPROM

;.------------------------------------------------------------------------.
;|                               MAIN                                     |
;`------------------------------------------------------------------------'

				.MODULE MAIN				
				.AREA   StartUp (ABS)
								
				.if TARGETROM == 1
					.ORG    ROMSTART
				.else
					.ORG	RAMSTART					
				.endif

Main:			
				.if TARGETROM == 1
					LDS		#STACKTOP		; must have stack!
				.endif
				
				; place your program here ********************
				
				ldaa	PACTL
				oraa	#0x80
				staa	PACTL				;make bit 7 used for output
				
				;turn on led0
				
				ldaa	PORTA
				oraa	#0x80
				staa	porta
				
				;wait 1000 ms
				ldab	#100
				jsr		dodelay
				
				;turn led0 off
				ldaa	porta
				anda	#0x7f
				
				;turn on led1
				oraa	#0b01000000
				staa	porta
				
				;wait 1000 ms
				ldab	#100
				jsr		dodelay
				
				;turn off led1
				ldaa	porta
				anda	#0b10111111
				
				;turn on led2
				oraa	#0b00100000
				staa	porta
				
				; wait 1000 ms
				ldab	#100
				jsr		dodelay
				
				;exit
				jmp		romstart
				
				;delays B * 10 ms
dodelay:		pshx
				psha
				tba
				beq		donedelay
delayouter:		ldx		#0x0800
delayloop:		dex
				bne		delayloop
				deca
				bne		delayouter
donedelay:		pula
				pulx
				rts
				
				; ********************************************

;.------------------------------------------------------------------------.
;|                            INCLUDES                                    |
;`------------------------------------------------------------------------'

;.------------------------------------------------------------------------.
;|                            CONSTANT  DATA                              |
;`------------------------------------------------------------------------'
				
;DumbTable:		.db		0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15

;Copyright:		.ascii	"Copyright (c) 2002 by the person that wrote it. "
;				.asciz	"Hands off."

;.------------------------------------------------------------------------.
;|                             VARIABLES                                  |
;`------------------------------------------------------------------------'

;.------------------------------------------------------------------------.
;|                           RESET VECTOR                                 |
;`------------------------------------------------------------------------'

				.AREA	RESETVEC (ABS)

				.if TARGETROM == 1
					.org	RESETVEC		 ;Place the reset vector so that
					.DW		Main		     ;we can run this from power-up.
				.endif
				