"""
The server I want is one that returns a page with a list of the given
header parameters, whatever the URL is.

上のプロンプトで大体Codegemma:7bに書いてもらった。キーボード割込みだけ付け足した。
"""
from http.server import BaseHTTPRequestHandler, HTTPServer


class MyHandler(BaseHTTPRequestHandler):
    def do_GET(self):
        self.send_response(200)
        self.send_header('Content-Type', 'text/html')
        self.end_headers()
        self.wfile.write(b'<html><body><table>')
        for header in self.headers:
            self.wfile.write(
                f'<tr><th>{header}</th>'
                f'<td>{self.headers[header]}</td></tr>'
                .encode())
        self.wfile.write(b'</table></body></html>')


if __name__ == '__main__':
    server_address = ('localhost', 8080)
    httpd = HTTPServer(server_address, MyHandler)
    print('Starting server...')
    try:
        httpd.serve_forever()
    except KeyboardInterrupt:
        httpd.shutdown()
        httpd.server_close()
        print("Bye.")
