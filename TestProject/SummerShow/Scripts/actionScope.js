//作用域
//12、作用域
//Js的作用域是函数作用域，我们的浏览器就是一个被实例化的window对象，如果在window下定义一个name字段，那么name字段就具有window这个函数作用域，也就是在window下都是可以访问的，如果在window下定义一个function test，然后里面再定义一个name，那么这个新定义的name只能在test函数下通用，而老的name继续在window下通用。
//用实例来说明
var name = 'first';
function test(){
    var name = 'second'
    console.log(name);
}
test();
console.log(name);
//输出：second,first(可以看到定义的变量作用域是分开的, 局部变量的优先级比同名的全局变量高)

var name = 'first';
function test(){
    name = 'second'
    console.log(name);
}
test();
console.log(name);
//输出：second, second(如果直接赋值，解析器从函数中未找到变量时，会尝试去上一层去寻找)

var name = 'first';
function test(){
    nam = 'second'
    console.log(nam);
}
test();
console.log(name);
//输出：second, first (如果直接赋值，同时变量又不存在，解析器从函数中未找到变量时，会尝试去上一层去寻找。未找到时，会隐式的定义一个window域下的变量(与name作用与相同，即window.nam)。证据是如果这时直接在控制台直接执行nam会看到打出second

function test() {
    var x = 1;
    function man() {
        x = 100;
    }
    console.log(x);
}
test();
//输出：100(函数中声明的变量在整个函数中都有定义)

var x = 1;
function test() {
    console.log(x);
    var x = '20';
    console.log(x);
}
rain()
//输出：undefined,20(由于在函数test内局部变量x在整个函数体内都有定义（var x= 'rain-man'进行了声明），所以在整个rain函数体内隐藏了同名的全局变量x(即window.x在rain函数内是取不到的)。这里之所以会弹出'undefined'是因为，第一个执行alert(x)时，局部变量x尚未被初始化。)
//上面的test方法等同于下面方法
function test() {
    var x;
    console.log(x);
    x = '20';
    console.log(x);
}


//----------------------------------------------
//更直接的
var x = 10;

function fn() {
    var b = 20;
    console.log(x+b);  //这里x没有在fn函数中定义，则相当于是自由变量
}
//这里的自由变量 取值 需要到创建这个函数的作用域中取值
//例如
function show(f) {
    var x = 20;
    (function () {
        f();
    })();
}
show(fn);//fn在这里被调用 ，但是创建fn的作用域在 上一层作用域中创建
