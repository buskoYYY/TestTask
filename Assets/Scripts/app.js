function getTime() {
  fetch('https://yandex.com/time/sync.json')
    .then(response => response.json())
    .then(data => {
      const dateTime = new Date(data.time * 1000);
      // Отправляем время в Unity
      window.unityInstance.SendMessage('TimeSynchronizer', 'SetTime', dateTime.getTime());
    })
    .catch(error => console.error(error));
}