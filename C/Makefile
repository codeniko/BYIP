CCFLAGS = -Wall

all: byip

byip.o: byip.c byip.h
	gcc $(CCFLAGS) -c byip.c byip.h

byip: byip.o
	gcc $(CCFLAGS) -o byip byip.o

clean:
	rm -f *.o
	rm -f byip
