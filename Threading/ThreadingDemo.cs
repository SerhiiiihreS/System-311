﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System_311.Threading
{
    internal class ThreadingDemo
    {
        private int nThreads;
        public void Run()
        {
            //Thread thread = new(ThreadMethod);
            //thread.Start();
            //thread.Join();   // очікування завершення потоку - блокується потік, що 
            //                 // викликає операцію (батьківській потік)
            Console.WriteLine("Homework");
            int N = 10;

            // якщо необхідно чекати кілька потоків, то чекати треба кожен
            Thread[] threads = new Thread[N];
            nThreads = N;
            for (int i = 0; i < N; i++)
            {
                threads[i] =                   // створення об'єкту не запускає потік
                    new Thread(CalcOneMonth);
                threads[i]                     // це робить Start, також дані до потоку 
                    .Start(i + 1);             // (аргумент методу) передається у Start

                // threads[i].Join();          // у даному місці - неправильно
            }
            //for (int i = 0; i < N; i++)       // Очікування всіх потоків - очікування
            //{                                 // кожного АЛЕ після того, як запущені ВСІ
            //    threads[i].Join();            // Якщо потік вже завершений, то виклик
            //}                                 // Join() суть ігнорується (без проблем)
            // // Очікування дозволяє організувати "точку", у якій є підсумковий результат
            // Console.WriteLine("-------------------");
            // Console.WriteLine("Total sum = {0:F2}", sum);
        }

        public string? rthred;
        private object strLocker = new();
        // об'єкт, створений заради критичної секції
        private void CalcOneMonth(object? thread)
        {

            // null-coalescence форма скороченого тернарного виразу
            // y = (x!=null) ? x : -1; -->  y = x ?? -1;  y = a ?? b ?? c ?? -1;
            // null-propagation/null-forgiving  x?.prop  --> (x==null) ? null : x.prop
            //  a?.b?.c ?? -1
            // null-checking   a!  -->  (a!=null) ? a : throw new NullRef..()
            //
            // x ??= -1;   x = (x!=null) ? x : -1
            int m = (int)(thread ?? -1 );
            Thread.Sleep(300);        // Імітація запиту АРІ
            string? strLocal;
            string rthredLocal;
            string? c;
            lock (strLocker)
            {
                if (m == 10)
                {
                    c = "0";
                }
                else
                {
                    c = Convert.ToString(m);
                }
                    strLocal = rthred += c;
            }
            Console.WriteLine("Thread {1}:  {0:F1}", strLocal, m);

            lock (this)   // для синхронізації можна використовувати інші об'єкти
            {
                nThreads--;
                if (nThreads == 0)
                {
                    Console.WriteLine("-------------------");
                    Console.WriteLine("Total: {0:F2}", strLocal);
                }
            }
        }
    }
}

/* Асинхронне програмування
 * 
 * Синхронність (у виконанні коду) - наявність послідовності (у часі)
 * 11111-
 *       222222-
 *              333333
 * Асинхронність - будь-яке відхилення від синхронності
 * 11111             1   1   ...        11111-3333333
 * 222222             2   2  ...        2222222
 * 333333              3   3 ...        
 * 
 * Способи реалізації асинхронності
 * - багатозадачність - елементи рівня мови програмування (функції/об'єкти)
 *                       Task, Promise, Future, Coroutine
 * - багатопоточність - елементи рівня операційної системи Thread(s)
 *                       (за умови, що ОС їх підтримує)
 * - процеси (мікросервіси) - елементи рівня устаткування - процесорів
 *                              у ARM замінюють потоки
 * - мережні технології (grid, network, cloud)
 */

/* Приклад:
 * Банк публікує дані щодо місячного рівня інфляції (кожен місяць)
 * Задача: визначити рівень інфляції за певний період (наприклад, за рік)
 * Складність: запит до банку тривалий (вимагає підключення, передачі даних)
 *  але можливо надсилати кілька запитів одночасно.
 * Аналіз: віповідаємо на питання "чи необхідно впорядковувати дані, чи 
 *  їх можна використати у довільному порядку?"
 * (100 + 10%) + 20% =/= 100 + 30%
 * (100 + 10%) + 20% =?= (100 + 20%) + 10% 
 * 100 x 1.1 x 1.2  ==  100 x 1.2 x 1.1
 */

/* Потоки. Багатопоточність.
 * Утворення:
 *  - оголошуємо метод. Метод не повертає значень. Параметри або
 *     відсутні, або один узагальнений (object)
 *  - створюємо об'єкт класу Thread, передаємо до конструктора метод
 *     ! створення об'єкту не запускає виконання. Для запуску 
 *     викликається метод Start()
 *  - вводимо "глобальну" обмінну змінну, роль якої - передача даних
 *     з середини потокового методу, змінюємо її значення за результатами
 *     роботи потоку
 *  - виявляємо ділянки, що стосуються роботи з глобальною областю, 
 *     синхронізуємо їх (lock), якщо глобальна область має Value-Type, то
 *     для синхронізації створюємо окремий об'єкт (найпростішого типу - object)
 *  !! не повинно бути коду, який використовує спільні об'єкти, поза 
 *      синхроблоками
 *     
 * Недоліки потоків
 * - неможливість повернення значення
 * - незручність передачі значення
 * - необхідність глобальної/спільної області для обміну даними
 * - необхідність синхронізації при роботі зі спільними даними
 */

/* Синхронізація - створення ситуацій, які унеможливлюють 
 * одночасну роботу фрагментів кількох кодів (задач, потоків, процесів)
 * 
 * sum *= 1.0 + percent / 100; Роберемо детально послідовність інструкції
 *  => sum = sum * (1.0 + percent / 100);
 * обчислення sum -> обчислення 1.0 + percent / 100 -> запис у sum (присвоєння) 
 * 
 * Зобразимо паралельну роботу кількох потоків (зі зміщеннями через випадкові фактори)
 * sum -> 1.0 + percent / 100 -> sum
 *           sum -> 1.0 + percent / 100 -> sum
 *           |                      sum -> 1.0 + percent / 100 -> sum
 *           |
 *           |У цей момент здійснюється "читання" sum для другого потоку,
 *            але ще не відбувся запис результату першого потоку
 * 
 *  sum -> 1.0 + percent / 100 -> sum
 *  |-------------------------------|          
 * Синхронізація - "блокування" послідовності операцій для паралельного 
 * виконання
 * 
 * |sum  ->  1.0 + percent / 100  ->  sum
 * |          |--------очікування--------|sum -> 1.0 + percent / 100 -> sum
 * |          |другий потік намагається  |
 * |           працювати, але "бачить"   |
 * |           блокування                |
 * |                                     |  
 * |блокування                           |розблокування - другий потік відновлює роботу
 * 
 * У C#, як і у багатьох ООП мовах, кожен об'єкт (Reference-Type) має власний
 * примітив синхронізації - критичну секцію, використання якої регулюється 
 * спеціальним блоком коду (lock, synchronized, ...)
 * 
 *    sleep  lock   cw
 *     -----| |-| |---
 * --< -----|-| | |---
 *     -----| | |-|---
 */


/* Д.З. Згенерувати випадкову перестановку цифр "0123456789" за
 * допомогою багатопоточної програми:
 * запускається 10 потоків, кожен з яких додає до спільного рядка одну цифру
 * а також виводить проміжний результат
 * У кінці роботи потоків виводиться підсумковий результат.
 * 
 * Очікуваний приклад:
 * thread 7:  "7"
 * thread 3:  "73"
 * thread 5:  "735"
 * ....
 * ---------------
 * total: "7350124689"
 * 
 */