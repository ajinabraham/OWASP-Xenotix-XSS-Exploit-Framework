from Xenotix import FUZZ_GET,GET
y=GET("http://opensecurity.in",headers=None,proxy=None,proxyauth=None,debug=0)
print y['HEADERS']
GBROWSER_RENDER=y['RESPONSE']
x=FUZZ_GET("http://www.dolby.com/us/en/sitesearch/index.aspx?q=[X]")
IBROWSER_NAVIGATE=x
