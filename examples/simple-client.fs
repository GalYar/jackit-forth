\ Copyright (c) 2009 Marcus Eskilsson. All rights reserved.

\ Redistribution and use in source and binary forms, with or without
\ modification, are permitted provided that the following conditions
\ are met:
\ 1. Redistributions of source code must retain the above copyright
\    notice, this list of conditions and the following disclaimer.
\ 2. Redistributions in binary form must reproduce the above copyright
\    notice, this list of conditions and the following disclaimer in the
\    documentation and/or other materials provided with the distribution.

\ THIS SOFTWARE IS PROVIDED BY AUTHOR AND CONTRIBUTORS ``AS IS'' AND
\ ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
\ IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
\ ARE DISCLAIMED.  IN NO EVENT SHALL AUTHOR OR CONTRIBUTORS BE LIABLE
\ FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
\ DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS
\ OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
\ HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
\ LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY
\ OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF
\ SUCH DAMAGE.

( --------------
  - This is a very simple client that demonstrates the basic
  - features of JACK.
  - Also it is a blatant ripoff of 
  - http://jackit.sourceforge.net/cgi-bin/lxr/http/source/example-clients/simple_client.c
  -------------- )

include jack.fs

variable input-port
variable output-port
0 value jack-client

s" simple gforth client" jack-client-name jnc

: process  ( n a -- n )
	\ TODO This needs to be using lib.fs
	\ The process callback for this JACK application.
	\ It is called by JACK at the appropriate times.
	drop				\ void *arg
	dup	dup				\ nframes
	inpu-port swap jack-port-get-buffer
	swap				\ in
	output-port swap jack-port-get-buffer
	\ sizeof(jack_default_audio_sample_t) hopefully == 4
	rot 2 lshift move			\ memcpy(out, in, ...)
	0
;
	

: client-open  ( -- n )
	jnc 0 0 jack-client-open
;
: jackit  ( -- )
	\ Try to become a client of the JACK server
	client-open to jack-client
	jack-client 0= if
		." connecting to jack server failed" bye
	else
		\ Tell the JACK server to call 'PROCESS' whenever
		\ there is work to be done.
		jack-client ' process 0 jack-set-process-callback
	then
;