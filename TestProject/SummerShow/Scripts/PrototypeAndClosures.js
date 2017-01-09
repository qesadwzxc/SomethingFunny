//js 原型和闭包
function show(x) {
    //值类型
    console.log(typeof (x));    // undefined
    console.log(typeof (10));   // number
    console.log(typeof ('abc')); // string
    console.log(typeof (true));  // boolean

    //引用类型
    console.log(typeof (function () { }));  //function

    console.log(typeof ([1, 'a', true]));  //object
    console.log(typeof ({ a: 10, b: 20 }));  //object
    console.log(typeof (null));  //object
    console.log(typeof (new Number(10)));  //object
}


//函数作为对象被赋值属性  ,属性可以是一个函数
var fn = function () { };
fn.a = 10;
fn.b = function () { };
fn.c = {
    name: "a",
    year:1990
}

//对象都是通过函数来创建
function Fn() {
    this.name = 'a';
}
var fn = new Fn();

var obj = { a: 10, b: 20 }; //实则相当于 
var obj = new Object();
obj.a = 10;
obj.b = 20;

//javascript 中的继承是通过 原型链: 访问一个对象属性，现在基本属性中查找
//，如果没有，则沿着_proto_这条链向上找
function Foo() { }
var f1 = new Foo();
f1.a = 10;
Foo.prototype.a = 100;
Foo.prototype.b = 200;

console.log(f1.a);// 10   f1的基本属性
console.log(f1.b);// 200  

//对象或者函数，在new出来之后，可能是空属性
JQuery = function (selector, context) {
    return new jQuery.fn.init(selector, context);
}
JQuery.extend({
    noop: function () { },
    isReady:true
})

//可以自行修改方法
function Foo() { }
var f1 = new Foo();
Foo.prototype.toString = function () {
    return "a";
}
console.log(f1.toString());// a
//但是在添加内置方法的原型属性，做判断该属性是否存在 hasOwnProperty(属性名)
//函数每被调用一次，都会产生一个新的执行上下文环境

//函数在定义的时候（不是调用的时候），就已经确定了函数体内部自由变量的作用域
var a = 10;
function fn() {
    console.log(a);//a是自由变量
}

function bar(f) {
    var a = 20;
    f();//打印 10 
}
bar(fn)

//-------------------------------------------------------------------------
//在函数中this，是在函数真正被调用执行的时候确定的，函数定义的时候确定不了

//this指向调用该函数的对象

//函数在被调用的时候会意外接受两个参数：this和argument，其中this的值跟取决于函数的调用模式
//1，方法调用模式 o.a() //this指的o
//2，函数调用模式 a() //this指的windows
//3，构造器调用模式 new a() //this为a的实例对象
//4，apply(call)的间接调用模式 a.apply(xx,[yy]) //this指的xx 

//情况1：构造函数
function Foo() {
    this.name = "a";
    this.year = 1988;
    console.log(this);//Foo {name:"a",year:1988}
}
var f1 = new Foo();//函数作为构造函数使用，则this为即将new出来的对象
Foo();//此时不new，则this 为windowd对象

//情况2：函数作为对象的一个属性
var obj = {
    x: 10,
    fn: function () {
        console.log(this);
                          //Object {x:10,fn:function}
        console.log(this.x);//10
    }
}
obj.fn;//此时函数为对象的 一个属性被调用时 this为该对象
var fn1 = obj.fn;
fn1();//fn1相当于是一个新的函数   fn函数被赋值到另一个变量中，没有作为obj的属性被调用，则this的值为window，this.x = undefined

//情况3：函数用calk和apply调用时 this的值就是传入对象的值
var obj = {
    x:10
}
var fn = function () {
    console.log(this);
    console.log(this.x);
}
fn.call(obj);

//情况4： 全局和普通函数 this永远为window

//----------------------------------------------------------------------------------
//执行上下文栈
var a = 10, fn                      //进入全局上下文
        , bar = function (x) {
            var b = 5;
            fn(x + b);              //进入fn函数上下文环境
        };
fn = function (y) {
    var c = 5;
    console.log(y+c);
}

bar(10);                            //进入bar函数上下文环境


//哎呀，终于到闭包了*****************************************************
//闭包：函数作为返回值，函数作为参数传递

//函数作为返回值
function fn() {
    var max = 10;

    return function bar(x) {
        if (x > max) {
            console.log(x);
        }
    }
}
var f1 = fn();
f1(15);

//函数作为参数
var max = 10,
    fn = function (x) {
        if (x > max) {
            console.log(x);
        }
    };
(function (f) {
    var max = 100;
    f(15);//此时 max=10 而不是100
})(fn);






