import React from 'react'
import './App.css'

function App() {
  return (
    <div className='app'>
      <div className='header'>
        <a href='http://'>Регистрация</a>
      </div>

      <div className="form_box">
        <h2>Авторизация</h2>
        <form>
          <lable>Имя пользователя/Email</lable>
          <input type='text'></input><br />

          <label>Пароль</label>
          <input type='password'></input><br />

          <label>Должность</label>
          <input type='select'></input><br />

          <input type='checkbox' /><label>Запомнить меня</label><br />
          <input type='submit' value='Вход'></input>
          <a href='http://'>Забыли пароль?</a><br />
          <a class='link_registration' href='http://'>Нужен аккаунт? Зарегистрируйтесь!</a>
        </form>
      </div>
    </div>
  )
}

export default App
