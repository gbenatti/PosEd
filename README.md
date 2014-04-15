Gerador de fases
================

O projeto do gerador foi desenvolvido em c# e pode ser compilado no mac os x utilizando o Xamarim Studio

Executando o programa você tem algumas teclas que podem ser utilizadas.

* r - randomiza/gera uma nova fase.
* b - modo de blocos, mostra as salas definidas no arquivo rooms.txt, no lugar das salas vazias. 
* g - modo jogo, mostra o jogo na forma como o mesmo rodaria, aproximadamente uma sala e meia preenchendo a tela.
* s - mostra as linhas do grid, bom para impressão da tela para prototipo.
* [ e ] - aumentam e diminuem o modo de zoom.

Arquivo de definição de salas
=============================

As salas utilizadas na geração da fase podem ser definidas através de quais paredes são fechadas: Top, Bottom, Left and Right 
em várias combinações possiveis.

E quando executamos o jogo em "modo de blocos", algumas templates são utilizadas no lugar de cada sala, esses templates são 
definidos no arquivo Rooms.txt.

Exemplo da seção de salas que são fechadas no topo, retirada do arquivo.

>Top
1111111111111111
1000000000000001
0011111111111100
0111111111111110
0111111111111110
0011111111111100
0000000000000000
0000000000000000
0000000000000000
-
1111111111111111
0110011111100000
0000001111000000
0000000000000000
0000000000000000
0000000000000000
0000000000000000
0000000000000000
0000000000000000

Cada sala é composta de 16 colunas com 9 linhas, contendo um caracter.
No momento são suportados os caracteres 0 e 1, onde:
 0 indica um espaço vazio.
 1 indica um espaço com terreno.

Editor de salas
===============

Para crias essas salas de 16x9 foi criada uma ferramentinha visual de geração.

http://jsbin.com/miluf/1

