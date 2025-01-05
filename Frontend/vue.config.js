module.exports = {
  devServer: {
    proxy: {
      '^/users': {
        target: 'http://localhost:7075/',
        ws: true,
        changeOrigin: true
      },
    }
  }
}